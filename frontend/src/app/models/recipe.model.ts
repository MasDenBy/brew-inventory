export interface RecipeListResponse {
  id: number;
  name: string;
  brewfatherId?: string;
}

export interface RecipeDetailsResponse {
  id: number;
  name: string;
  brewfatherId?: string;
  style?: string;
  fermentables: RecipeFermentableDetail[];
  hops: RecipeHopDetail[];
  yeasts: RecipeYeastDetail[];
  miscs: RecipeMiscDetail[];
}

export interface RecipeFermentableDetail {
  name: string;
  amount: number;
  type: string;
  supplier?: string;
  origin?: string;
  color?: number;
  potential?: number;
}

export interface RecipeHopDetail {
  name: string;
  amount: number;
  alpha?: number;
  type: string;
  origin?: string;
  use: string;
  time?: number;
}

export interface RecipeYeastDetail {
  name: string;
  amount: number;
  laboratory?: string;
  type: string;
  form: string;
  attenuation?: number;
  unit?: string;
}

export interface RecipeMiscDetail {
  name: string;
  amount: number;
  type: string;
  unit?: string;
  use?: string;
  time?: number;
}

export interface CreateRecipeRequest {
  name: string;
  style?: string;
  fermentables: CreateRecipeFermentableRequest[];
  hops: CreateRecipeHopRequest[];
  yeasts: CreateRecipeYeastRequest[];
  miscs: CreateRecipeMiscRequest[];
}

export interface CreateRecipeFermentableRequest {
  name: string;
  amount: number;
  type: string;
  supplier?: string;
  origin?: string;
  color?: number;
  potential?: number;
}

export interface CreateRecipeHopRequest {
  name: string;
  amount: number;
  alpha?: number;
  type: string;
  origin?: string;
  use: string;
  time?: number;
}

export interface CreateRecipeYeastRequest {
  name: string;
  amount: number;
  laboratory?: string;
  type: string;
  form: string;
  attenuation?: number;
  unit?: string;
}

export interface CreateRecipeMiscRequest {
  name: string;
  amount: number;
  type: string;
  unit?: string;
  use?: string;
  time?: number;
}

export interface SyncRecipeResponse {
  id: number;
  name: string;
  brewfatherId: string;
}
