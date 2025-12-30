export enum YeastType {
  Ale = 'Ale',
  Lager = 'Lager',
  Hybrid = 'Hybrid',
  Wheat = 'Wheat',
  Wine = 'Wine',
  Champagne = 'Champagne',
  Other = 'Other'
}

export enum YeastForm {
  Liquid = 'Liquid',
  Dry = 'Dry',
  Culture = 'Culture',
  Slurry = 'Slurry'
}

export interface Yeast {
  id: number;
  name: string;
  laboratory: string;
  type: YeastType;
  form: YeastForm;
  amount: number;
  bestBefore: string | null;
  brewfatherId: string | null;
}
