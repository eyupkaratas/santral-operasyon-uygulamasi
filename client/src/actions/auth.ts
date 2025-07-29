"use server";

import { cookies } from "next/headers";

export async function loginAction(formData: {
  email: string;
  password: string;
}): Promise<LoginResponse | ErrorResponse> {
  const cookieStore = await cookies();

  const res = await fetch("http://localhost:5183/api/Auth/login", {
    method: "POST",
    body: JSON.stringify({
      eposta: formData.email,
      sifre: formData.password,
    }),
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    const error: ErrorResponse = await res.json();
    return {
      success: false,
      status: error.status,
      message: error.message,
    };
  }

  const data: LoginResponse = await res.json();

  cookieStore.set("token", data.token, {
    path: "/",
    sameSite: "strict",
    maxAge: 60 * 60 * 24,
  });

  return {
    success: true,
    token: data.token,
  };
}
