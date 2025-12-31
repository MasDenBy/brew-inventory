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
