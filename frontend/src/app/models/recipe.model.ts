export interface RecipeListResponse {
  id: number;
  name: string;
  brewfatherId?: string;
}

export interface RecipeDetailsResponse {
  id: number;
  name: string;
  brewfatherId?: string;
  fermentables: RecipeFermentableDetail[];
  hops: RecipeHopDetail[];
  yeasts: RecipeYeastDetail[];
  miscs: RecipeMiscDetail[];
}

export interface RecipeFermentableDetail {
  fermentableId: number;
  name: string;
  type: string;
  amount: number;
  supplier?: string;
  origin?: string;
  color: number;
}

export interface RecipeHopDetail {
  hopId: number;
  name: string;
  type: string;
  amount: number;
  origin?: string;
  alphaAcid: number;
  harvestYear?: number;
}

export interface RecipeYeastDetail {
  yeastId: number;
  name: string;
  type: string;
  form: string;
  amount: number;
  laboratory: string;
}

export interface RecipeMiscDetail {
  miscId: number;
  name: string;
  type: string;
  unit: string;
  amount: number;
}

export interface CreateRecipeRequest {
  name: string;
  fermentables: CreateRecipeFermentableRequest[];
  hops: CreateRecipeHopRequest[];
  yeasts: CreateRecipeYeastRequest[];
  miscs: CreateRecipeMiscRequest[];
}

export interface CreateRecipeFermentableRequest {
  fermentableId: number;
  amount: number;
}

export interface CreateRecipeHopRequest {
  hopId: number;
  amount: number;
}

export interface CreateRecipeYeastRequest {
  yeastId: number;
  amount: number;
}

export interface CreateRecipeMiscRequest {
  miscId: number;
  amount: number;
}

export interface SyncRecipeResponse {
  id: number;
  name: string;
  brewfatherId: string;
}
