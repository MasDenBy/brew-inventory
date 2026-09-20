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
  name: string;
  type: string;
  amountNeeded: number;
  amountInInventory: number;
  amountToBuy: number;
  unit: string | null;
  supplier: string | null;
  productId: string | null;
}
