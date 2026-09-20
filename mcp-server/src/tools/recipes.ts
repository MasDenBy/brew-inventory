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
  style: string | null;
  notes: string | null;
  fermentables: {
    name: string;
    amount: number;
    type: string;
    supplier: string | null;
    origin: string | null;
    color: number | null;
    potential: number | null;
  }[];
  hops: {
    name: string;
    amount: number;
    alpha: number | null;
    type: string;
    origin: string | null;
    use: string;
    time: number | null;
  }[];
  yeasts: {
    name: string;
    amount: number;
    laboratory: string | null;
    type: string;
    form: string;
    attenuation: number | null;
    unit: string | null;
  }[];
  miscs: {
    name: string;
    amount: number;
    type: string;
    unit: string | null;
    use: string | null;
    time: number | null;
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

      if (r.style) {
        lines.push(`Style: ${r.style}`);
      }

      if (r.notes) {
        lines.push(`Notes: ${r.notes}`);
      }

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
        "Create a new recipe with denormalized ingredient data.",
      inputSchema: z.object({
        name: z.string().min(1),
        style: z.string().optional(),
        notes: z.string().optional(),
        fermentables: z
          .array(
            z.object({
              name: z.string().min(1),
              amount: z.number().positive(),
              type: z.string(),
              supplier: z.string().optional(),
              origin: z.string().optional(),
              color: z.number().optional(),
              potential: z.number().optional(),
            }),
          )
          .default([]),
        hops: z
          .array(
            z.object({
              name: z.string().min(1),
              amount: z.number().positive(),
              alpha: z.number().optional(),
              type: z.string(),
              origin: z.string().optional(),
              use: z.string(),
              time: z.number().optional(),
            }),
          )
          .default([]),
        yeasts: z
          .array(
            z.object({
              name: z.string().min(1),
              amount: z.number().positive(),
              laboratory: z.string().optional(),
              type: z.string(),
              form: z.string(),
              attenuation: z.number().optional(),
              unit: z.string().optional(),
            }),
          )
          .default([]),
        miscs: z
          .array(
            z.object({
              name: z.string().min(1),
              amount: z.number().positive(),
              type: z.string(),
              unit: z.string().optional(),
              use: z.string().optional(),
              time: z.number().optional(),
            }),
          )
          .default([]),
      }),
    },
    async (input) => {
      const created = await api.post<RecipeDetails>("/api/recipes", {
        name: input.name,
        style: input.style,
        notes: input.notes,
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
