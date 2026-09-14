import type { McpServer } from "@modelcontextprotocol/server";
import { z } from "zod/v4";
import { api } from "../apiClient.js";

interface Fermentable {
  id: number;
  name: string;
  amount: number;
  bestBefore: string | null;
  brewfatherId: string | null;
  supplier: string | null;
  origin: string | null;
  type: string;
  color: number;
}

export function registerFermentableTools(server: McpServer) {
  server.registerTool(
    "list_fermentables",
    {
      description:
        "List all fermentable grains/sugars in the inventory with their amounts and details",
      inputSchema: z.object({}),
    },
    async () => {
      const items = await api.get<Fermentable[]>("/api/fermentables");
      const text =
        items.length === 0
          ? "No fermentables found in inventory."
          : items
              .map(
                (f) =>
                  `- [#${f.id}] ${f.name} | Amount: ${f.amount} | Type: ${f.type} | Color: ${f.color} | Supplier: ${f.supplier || "-"} | Origin: ${f.origin || "-"}`,
              )
              .join("\n");
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "get_fermentable",
    {
      description: "Get a single fermentable by its ID",
      inputSchema: z.object({ id: z.number().int().positive() }),
    },
    async ({ id }) => {
      const f = await api.get<Fermentable>(`/api/fermentables/${id}`);
      const text = `[#${f.id}] ${f.name}
Amount: ${f.amount}
Type: ${f.type}
Color: ${f.color}
Supplier: ${f.supplier || "-"}
Origin: ${f.origin || "-"}
BestBefore: ${f.bestBefore || "-"}`;
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "create_fermentable",
    {
      description:
        "Create a new fermentable in the inventory. Type is one of: Grain, Sugar, LiquidExtract, DryExtract, Adjunct, Other",
      inputSchema: z.object({
        name: z.string().min(1),
        amount: z.number().nonnegative(),
        type: z.string(),
        color: z.number().nonnegative().default(0),
        supplier: z.string().optional().nullable(),
        origin: z.string().optional().nullable(),
        bestBefore: z.string().optional().nullable(),
      }),
    },
    async (input) => {
      const created = await api.post<Fermentable>("/api/fermentables", {
        name: input.name,
        amount: input.amount,
        type: input.type,
        color: input.color,
        supplier: input.supplier ?? null,
        origin: input.origin ?? null,
        bestBefore: input.bestBefore ?? null,
        brewfatherId: null,
      });
      const text = `Created fermentable [#${created.id}] ${created.name} with amount ${created.amount} ${created.type}.`;
      return { content: [{ type: "text", text }] };
    },
  );
}