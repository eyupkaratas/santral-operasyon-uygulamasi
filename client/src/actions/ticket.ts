"use server";

import { ErrorResponse } from "@/types/error-response";
import { Ticket } from "@/types/ticket";
import { revalidatePath } from "next/cache";
import { cookies } from "next/headers";

const baseUrl = process.env.API_BASE_URL!;

export async function getTicketsAction() {
  const res = await fetch(`${baseUrl}/api/Tickets`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  });

  const data: Ticket[] = await res.json();
  return data;
}

export async function updateTicketAction(
  formData: { personnelId: string; status: string; priority: string },
  ticketId: number
) {
  const cookieStore = await cookies();

  const token = cookieStore.get("token");

  if (!token) {
    return {
      success: false,
      status: 401,
      message: "Yetkisiz erişim.",
    };
  }

  const res = await fetch(`${baseUrl}/api/Tickets/${ticketId}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token?.value}`,
    },
    body: JSON.stringify({
      atananPersonelId: Number(formData.personnelId),
      durum: Number(formData.status),
      oncelik: Number(formData.priority),
    }),
  });

  if (!res.ok) {
    const error: ErrorResponse = await res.json();
    return {
      success: false,
      status: error.status,
      message: error.message,
    };
  }

  revalidatePath("/talepler");

  return {
    success: true,
    message: "Talep başarıyla güncellendi.",
  };
}
