export enum CargoStatus {
  Beklemede = "Beklemede",
  TeslimEdildi = "Teslim Edildi",
}

export type Cargo = {
  id: number;
  gonderen: string;
  aciklama: string;
  durum: CargoStatus;
  createdDate: string;
  takipNumarasi: string;
  teslimTarihi: string | null;
  teslimAlanPersonelAdi: string | null;
};
