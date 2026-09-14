import type { McpServer } from "@modelcontextprotocol/server";
import { z } from "zod/v4";
import { api } from "../apiClient.js";

interface Hop {
  id: number;
  name: string;
  amount: number;
  bestBefore: string | null;
  brewfatherId: string | null;
  origin: string | null;
  type: string;
  alphaAcid: number;
  harvestYear: number | null;
}

export function registerHopTools(server: McpServer) {
  server.registerTool(
    "list_hops",
    {
      description:
        "List all hops in the inventory with their amounts and details",
      inputSchema: z.object({}),
    },
    async () => {
      const items = await api.get<Hop[]>("/api/hops");
      const text =
        items.length === 0
          ? "No hops found in inventory."
          : items
              .map(
                (h) =>
                  `- [#${h.id}] ${h.name} | Amount: ${h.amount} | Type: ${h.type} | AlphaAcid: ${h.alphaAcid}% | Origin: ${h.origin || "-"} | Harvest: ${h.harvestYear || "-"}`,
              )
              .join("\n");
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "get_hop",
    {
      description: "Get a single hop by its ID",
      inputSchema: z.object({ id: z.number().int().positive() }),
    },
    async ({ id }) => {
      const h = await api.get<Hop>(`/api/hops/${id}`);
      const text = `[#${h.id}] ${h.name}
Amount: ${h.amount}
Type: ${h.type}
AlphaAcid: ${h.alphaAcid}%
Origin: ${h.origin || "-"}
HarvestYear: ${h.harvestYear || "-"}
BestBefore: ${h.bestBefore || "-"}`;
      return { content: [{ type: "text", text }] };
    },
  );

  server.registerTool(
    "create_hop",
    {
      description:
        "Create a new hop in the inventory. Type is one of: Pellet, Whole, Cryo, CO2Extract",
      inputSchema: z.object({
        name: z.string().min(1),
        amount: z.number().nonnegative(),
        type: z.string(),
        alphaAcid: z.number().nonnegative().default(0),
        origin: z.string().optional().nullable(),
        harvestYear: z.number().int().optional().nullable(),
        bestBefore: z.string().optional().nullable(),
      }),
    },
    async (input) => {
      const created = await api.post<Hop>("/api/hops", {
        name: input.name,
        amount: input.amount,
        type: input.type,
        alphaAcid: input.alphaAcid,
        origin: input.origin ?? null,
        harvestYear: input.harvestYear ?? null,
        bestBefore: input.bestBefore ?? null,
        brewfatherId: null,
      });
      const text = `Created hop [#${created.id}] ${created.name} with amount ${created.amount} (${created.type}, ${created.alphaAcid}% AA).`;
      return { content: [{ type: "text", text }] };
    },
  );
}