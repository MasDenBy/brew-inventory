import { McpServer } from "@modelcontextprotocol/server";
import { serveStdio } from "@modelcontextprotocol/server/stdio";
import { registerFermentableTools } from "./tools/fermentables.js";
import { registerHopTools } from "./tools/hops.js";
import { registerYeastTools } from "./tools/yeasts.js";
import { registerMiscTools } from "./tools/miscs.js";
import { registerRecipeTools } from "./tools/recipes.js";

serveStdio(() => {
  const server = new McpServer({
    name: "brew-inventory",
    version: "1.0.0",
  });

  registerFermentableTools(server);
  registerHopTools(server);
  registerYeastTools(server);
  registerMiscTools(server);
  registerRecipeTools(server);

  return server;
});