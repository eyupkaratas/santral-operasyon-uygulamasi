"use server";

import { ErrorResponse } from "@/types/error-response";
import { RecordNumberResponse } from "@/types/record-number-response";
import { cookies } from "next/headers";

const baseUrl = process.env.API_BASE_URL!;

export async function createNumberRecordAction(formData: {
  no: string;
  direction: number;
  personnelId: number;
  notes: string;
  personName: string;
}): Promise<RecordNumberResponse | ErrorResponse> {
  const cookieStore = await cookies();

  const token = cookieStore.get("token");

  const res = await fetch(`${baseUrl}/api/Lookup`, {
    method: "POST",
    body: JSON.stringify({
      numara: formData.no,
      yonu: formData.direction,
      personelId: formData.personnelId,
      notlar: formData.notes,
      yeniKisiAdi: formData.personName,
    }),
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token?.value}`,
    },
  });

  if (!res.ok) {
    const error: ErrorResponse = await res.json();
    return {
      success: false,
      status: error.status,
      message: error.message,
    };
  }

  const data: RecordNumberResponse = await res.json();

  return {
    success: true,
    id: data.id,
  };
}
