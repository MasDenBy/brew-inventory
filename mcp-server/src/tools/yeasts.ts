import type { McpServer } from "@modelcontextprotocol/server";
import { z } from "zod/v4";
import { api } from "../apiClient.js";

interface Yeast {
  id: number;
  name: string;
  amount: number;
  bestBefore: string | null;
  brewfatherId: string | null;
  laboratory: string;
  type: string;
  form: string;
}

export function registerYeastTools(server: McpServer) {
  server.registerTool(
    "list_yeasts",
    {
      description:
        "List all yeasts in the inventory with their amounts and details",
      inputSchema: z.object({}),
    },
    async () => {
      const items = await api.get<Yeast[]>("/api/yeasts");
      const text =
        items.length === 0
          ? "No yeasts found in inventory."
          : items
              .map(
                (y) =>
                  `- [#${y.id}] ${y.name} | Amount: ${y.amount} | Lab: ${y.laboratory} | Type: ${y.type} | Form: ${y.form}`,
              )
              .join("\n");
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "get_yeast",
    {
      description: "Get a single yeast by its ID",
      inputSchema: z.object({ id: z.number().int().positive() }),
    },
    async ({ id }) => {
      const y = await api.get<Yeast>(`/api/yeasts/${id}`);
      const text = `[#${y.id}] ${y.name}
Amount: ${y.amount}
Laboratory: ${y.laboratory}
Type: ${y.type}
Form: ${y.form}
BestBefore: ${y.bestBefore || "-"}`;
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "create_yeast",
    {
      description:
        "Create a new yeast in the inventory. Type is one of: Ale, Lager, Hybrid, Wheat, Wine, Champagne, Other. Form is one of: Liquid, Dry, Culture, Slurry",
      inputSchema: z.object({
        name: z.string().min(1),
        amount: z.number().nonnegative(),
        laboratory: z.string().min(1),
        type: z.string().optional().nullable(),
        form: z.string().optional().nullable(),
        bestBefore: z.string().optional().nullable(),
      }),
    },
    async (input) => {
      const created = await api.post<Yeast>("/api/yeasts", {
        name: input.name,
        amount: input.amount,
        laboratory: input.laboratory,
        type: input.type ?? null,
        form: input.form ?? null,
        bestBefore: input.bestBefore ?? null,
        brewfatherId: null,
      });
      const text = `Created yeast [#${created.id}] ${created.name} (${created.laboratory}) with amount ${created.amount}.`;
      return { content: [{ type: "text", text }] };
    },
  );
}