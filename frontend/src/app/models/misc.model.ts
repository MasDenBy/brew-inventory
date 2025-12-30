export enum InventoryUnit {
  Grams = 0,
  Kilograms = 1,
  Liters = 2,
  Milliliters = 3,
  Packages = 4,
  Tablets = 5
}

export enum MiscType {
  Spice = 0,
  Herb = 1,
  Fruit = 2,
  Flavor = 3,
  WaterAgent = 4,
  Other = 5
}

export interface Misc {
  id: number;
  name: string;
  amount: number;
  unit: InventoryUnit;
  type: MiscType;
  bestBefore: string | null;
  brewfatherId: string | null;
}
