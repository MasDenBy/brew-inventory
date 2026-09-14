import type { McpServer } from "@modelcontextprotocol/server";
import { z } from "zod/v4";
import { api } from "../apiClient.js";

interface RecipeListEntry {
  id: number;
  name: string;
  brewfatherId: string | null;
}

interface RecipeDetails {
  id: number;
  name: string;
  brewfatherId: string | null;
  fermentables: {
    fermentableId: number;
    name: string;
    type: string;
    amount: number;
    supplier: string | null;
    origin: string | null;
    color: number;
  }[];
  hops: {
    hopId: number;
    name: string;
    type: string;
    amount: number;
    origin: string | null;
    alphaAcid: number;
    harvestYear: number | null;
  }[];
  yeasts: {
    yeastId: number;
    name: string;
    type: string;
    form: string;
    amount: number;
    laboratory: string;
  }[];
  miscs: {
    miscId: number;
    name: string;
    type: string;
    unit: string;
    amount: number;
  }[];
}

export function registerRecipeTools(server: McpServer) {
  server.registerTool(
    "list_recipes",
    {
      description: "List all recipes with their IDs and names",
      inputSchema: z.object({}),
    },
    async () => {
      const items = await api.get<RecipeListEntry[]>("/api/recipes");
      const text =
        items.length === 0
          ? "No recipes found."
          : items.map((r) => `- [#${r.id}] ${r.name}`).join("\n");
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "get_recipe",
    {
      description: "Get the full details of a recipe including all its ingredients",
      inputSchema: z.object({ id: z.number().int().positive() }),
    },
    async ({ id }) => {
      const r = await api.get<RecipeDetails>(`/api/recipes/${id}`);
      const lines = [`[#${r.id}] ${r.name}`];

      if (r.fermentables.length > 0) {
        lines.push("Fermentables:");
        lines.push(
          ...r.fermentables.map(
            (f) => `  - ${f.name} (${f.type}): ${f.amount}`,
          ),
        );
      }
      if (r.hops.length > 0) {
        lines.push("Hops:");
        lines.push(
          ...r.hops.map((h) => `  - ${h.name} (${h.type}): ${h.amount}`),
        );
      }
      if (r.yeasts.length > 0) {
        lines.push("Yeasts:");
        lines.push(
          ...r.yeasts.map((y) => `  - ${y.name} (${y.laboratory}): ${y.amount}`),
        );
      }
      if (r.miscs.length > 0) {
        lines.push("Miscs:");
        lines.push(
          ...r.miscs.map((m) => `  - ${m.name} (${m.type}): ${m.amount} ${m.unit}`),
        );
      }

      return { content: [{ type: "text", text: lines.join("\n") }] };
    },
  );

  server.registerTool(
    "create_recipe",
    {
      description:
        "Create a new recipe. Ingredients must reference existing inventory item IDs and amounts. Use list_fermentables/list_hops/list_yeasts/list_miscs to find valid IDs.",
      inputSchema: z.object({
        name: z.string().min(1),
        fermentables: z
          .array(
            z.object({
              fermentableId: z.number().int().positive(),
              amount: z.number().positive(),
            }),
          )
          .default([]),
        hops: z
          .array(
            z.object({
              hopId: z.number().int().positive(),
              amount: z.number().positive(),
            }),
          )
          .default([]),
        yeasts: z
          .array(
            z.object({
              yeastId: z.number().int().positive(),
              amount: z.number().positive(),
            }),
          )
          .default([]),
        miscs: z
          .array(
            z.object({
              miscId: z.number().int().positive(),
              amount: z.number().positive(),
            }),
          )
          .default([]),
      }),
    },
    async (input) => {
      const created = await api.post<RecipeDetails>("/api/recipes", {
        name: input.name,
        fermentables: input.fermentables,
        hops: input.hops,
        yeasts: input.yeasts,
        miscs: input.miscs,
      });

      const ingredients = [
        created.fermentables.length,
        created.hops.length,
        created.yeasts.length,
        created.miscs.length,
      ].reduce((a, b) => a + b, 0);

      const text = `Recipe "[#${created.id}] ${created.name}" created with ${ingredients} ingredients.`;
      return { content: [{ type: "text", text }] };
    },
  );
}