---
name: recipe-assistant
description: "Analyzes beer recipe screenshots via OCR, extracts the ingredients, matches them against the brew-inventory API, proposes substitutions for missing ingredients in the format 'recipe ingredient - proposed ingredient', and creates the finalized recipe in the API. USE FOR: recipe screenshot import, OCR a recipe, match recipe ingredients to inventory, propose ingredients, create a recipe from an image, check ingredient availability from a recipe."
---

# Recipe Assistant

This skill analyzes a beer recipe screenshot, extracts its ingredients using vision OCR, matches them against the local inventory via the brew-inventory MCP tools, gets the user's approval on matches/substitutions, and finally creates the recipe in the API. The user expects an interactive, step-by-step CLI experience. **Do not skip the user-approval step.**

## Prerequisites

- The brew-inventory API is running (default `http://localhost:5000`).
- The brew-inventory MCP server is connected (tools are prefixed `brew_inventory_*`).
- The user provides a recipe screenshot either as a **file path** (PNG/JPG/WebP) or by pasting/attaching the image directly into the chat (supported in the opencode Web/IDE UIs).

## Workflow

### Step 1: OCR the Screenshot

Operate on the image the user supplied:

- If the user pasted/attached the image into the message, it is already visible to you — use it directly, no path needed.
- If the user gave a file path, read that image file first.

The model's vision capability is the OCR engine.

1. Look at the screenshot and extract the recipe **name** (if visible).
2. Extract every ingredient line exactly as written (name, amount, unit).
3. Classify each ingredient into one of four categories using context:
   - **Fermentable** - grains, sugar, extracts, adjuncts
   - **Hop** - hops (typically includes alpha acid % and/or "boil/whirlpool/dry hop" times)
   - **Yeast** - yeast strains and lab names
   - **Misc** - finings (Irish Moss, Whirlfloc), spices, water agents, other
4. Present the extracted list to the user for confirmation before proceeding.

### Step 2: Query Inventory

Call the MCP tools to fetch the full inventory (all four categories):

```
brew_inventory_list_fermentables
brew_inventory_list_hops
brew_inventory_list_yeasts
brew_inventory_list_miscs
```

Keep this inventory data in context - it is the source of truth for matching.

### Step 3: Match Ingredients

For each recipe ingredient, find the best match in inventory:

1. **Exact / near-exact name match** (e.g. "Pale Ale Malt" vs "Pale Ale Malt") - direct match.
2. **Synonym / variant match** (e.g. "2-Row" vs "Pale Malt", "US-05" vs "Safale US-05", "Cascade" vs "Cascade Whole Leaf") - match, note it as a variant.
3. **Substitute match within the same ingredient category** when the exact item is absent - propose it clearly as a substitute (e.g. "Munich" missing, propose "Vienna"; "Irish Moss" missing, propose "Whirlfloc"). Prefer substitutes with the same role/type: grain for grain, bittering hop for bittering hop, ale yeast for ale yeast, fining for fining.
4. **No match** - mark as missing and ask the user how to proceed.

Compare **amounts**: if the matched inventory item has less than the recipe needs, flag it as INSUFFICIENT and state how much is missing.

### Step 4: Present Proposals (User Approval Required)

Present each ingredient using this exact format:

```
✅ [recipe ingredient ] -> [matched inventory ingredient]  (Amount: OK / INSUFFICIENT - missing X)
⚠️ [recipe ingredient ] -> [substitute inventory ingredient] (Substitute, confirm?)
❓ [recipe ingredient ] -> NOT FOUND
```

Then wait for the user. They may:
- Confirm all matches (type "confirm" / "ok").
- Accept or reject individual substitutes.
- Pick a different substitute from the list you present.
- Choose to skip an ingredient.
- Ask to add a missing ingredient to inventory first (use `brew_inventory_create_*` tools), then retry the match.

In a single output message, present the full proposal table plus the inline questions for any `❓` or `⚠️` items.

### Step 5: Create the Recipe

Only after the user confirms the final ingredient list, create the recipe in the API:

```
brew_inventory_create_recipe
```

Pass the confirmed matches using each inventory item's numeric ID (from the list tool output) and amounts from the recipe (not inventory amounts):

- `name`: recipe name from the screenshot
- `notes`: the full raw OCR text of the recipe (preserve original formatting)
- `fermentables[]`: `{ fermentableId, amount }`
- `hops[]`: `{ hopId, amount }`
- `yeasts[]`: `{ yeastId, amount }`
- `miscs[]`: `{ miscId, amount }`

Report the created recipe ID and name, and note which ingredients were substituted.

## Rules

- **Never create the recipe without explicit user confirmation** of every non-exact match.
- Keep amounts in the units as written in the recipe; do not convert unless the user asks.
- If the API returns an error (e.g. invalid ingredient ID), surface it plainly and ask the user how to resolve it.
- If inventory is empty, tell the user the inventory is empty and suggest syncing first, then stop.
- If any `create_*` tool is needed to add a new ingredient before finalizing, complete that first, refresh the matching, and continue.