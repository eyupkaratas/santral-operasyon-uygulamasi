"use server";

import { Personnel } from "@/types/personnel";

const baseUrl = process.env.API_BASE_URL!;

export async function getPersonnelsAction(birimId?: string) {
  const params = new URLSearchParams();

  if (birimId) params.append("birimId", birimId.toString());

  const res = await fetch(`${baseUrl}/api/Personeller?${params.toString()}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  });

  const data: Personnel[] = await res.json();
  return data;
}
