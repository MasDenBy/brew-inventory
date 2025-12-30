export enum IngredientType {
    Hops = 'Hops',
    Fermentable = 'Fermentable',
    Yeasts = 'Yeasts',
    Misc = 'Misc'
}

export interface Ingredient {
    id: number;
    name: string;
    supplier: string;
    amount: number;
    expirationDate: Date;
    type: IngredientType;
}
