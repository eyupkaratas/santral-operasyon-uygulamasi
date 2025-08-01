"use server";

import { Cargo } from "@/types/cargo";

const baseUrl = process.env.API_BASE_URL!;

export async function getCargosAction() {
  const res = await fetch(`${baseUrl}/api/Kargolar`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  });

  const data: Cargo[] = await res.json();
  return data;
}
