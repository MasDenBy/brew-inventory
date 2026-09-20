export enum FermentableType {
  Grain = 'Grain',
  Sugar = 'Sugar',
  Extract = 'Extract',
  DryExtract = 'DryExtract',
  Adjunct = 'Adjunct',
  Other = 'Other'
}

export interface Fermentable {
  id: number;
  name: string;
  amount: number;
  brewfatherId: string | null;
  supplier: string | null;
  origin: string | null;
  type: FermentableType;
  color: number;
  grainCategory: string | null;
  percentage: number | null;
  lovibond: number;
}
