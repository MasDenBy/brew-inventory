import type { McpServer } from "@modelcontextprotocol/server";
import { z } from "zod/v4";
import { api } from "../apiClient.js";

interface Misc {
  id: number;
  name: string;
  amount: number;
  unit: string;
  type: string;
  brewfatherId: string | null;
}

export function registerMiscTools(server: McpServer) {
  server.registerTool(
    "list_miscs",
    {
      description:
        "List all miscellaneous ingredients (spices, finings, water agents, etc.) in the inventory",
      inputSchema: z.object({}),
    },
    async () => {
      const items = await api.get<Misc[]>("/api/miscs");
      const text =
        items.length === 0
          ? "No misc ingredients found in inventory."
          : items
              .map(
                (m) =>
                  `- [#${m.id}] ${m.name} | Amount: ${m.amount} ${m.unit} | Type: ${m.type}`,
              )
              .join("\n");
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "get_misc",
    {
      description: "Get a single misc ingredient by its ID",
      inputSchema: z.object({ id: z.number().int().positive() }),
    },
    async ({ id }) => {
      const m = await api.get<Misc>(`/api/miscs/${id}`);
      const text = `[#${m.id}] ${m.name}
Amount: ${m.amount} ${m.unit}
Type: ${m.type}`;
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "create_misc",
    {
      description:
        "Create a new misc ingredient in the inventory. Unit is one of: Grams, Kilograms, Liters, Milliliters, Packages, Tablets. Type is one of: Spice, Herb, Fruit, Flavor, WaterAgent, Other, Fining",
      inputSchema: z.object({
        name: z.string().min(1),
        amount: z.number().nonnegative(),
        unit: z.string(),
        type: z.string().optional().nullable(),
      }),
    },
    async (input) => {
      const created = await api.post<Misc>("/api/miscs", {
        name: input.name,
        amount: input.amount,
        unit: input.unit,
        type: input.type ?? null,
        brewfatherId: null,
      });
      const text = `Created misc ingredient [#${created.id}] ${created.name} with amount ${created.amount} ${created.unit}.`;
      return { content: [{ type: "text", text }] };
    },
  );
}
