export interface IngredientPurchaseRequest {
  recipeIds: number[];
}

export interface IngredientPurchaseResponse {
  fermentables: IngredientNeedDetail[];
  hops: IngredientNeedDetail[];
  yeasts: IngredientNeedDetail[];
  miscs: IngredientNeedDetail[];
}

export interface IngredientNeedDetail {
  ingredientId: number;
  name: string;
  type: string;
  amountNeeded: number;
  amountInInventory: number;
  amountToBuy: number;
  unit: string | null;
}
