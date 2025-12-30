export enum HopType {
  Pellet = 'Pellet',
  Whole = 'Whole',
  Cryo = 'Cryo',
  CO2Extract = 'CO2Extract'
}

export interface Hop {
  id: number;
  name: string;
  amount: number;
  bestBefore: string | null;
  brewfatherId: string | null;
  origin: string | null;
  type: HopType;
  alphaAcid: number;
  harvestYear: number | null;
}
