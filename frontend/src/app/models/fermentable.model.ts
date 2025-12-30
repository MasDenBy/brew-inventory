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
  bestBefore: string | null;
  brewfatherId: string | null;
  supplier: string | null;
  origin: string | null;
  type: FermentableType;
  color: number;
}
