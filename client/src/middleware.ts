import { jwtVerify } from "jose";
import { NextRequest, NextResponse } from "next/server";
import { DecodedToken } from "./types/decoded-token";

const publicRoutes = ["/anasayfa"];
const adminRoutes = [""];
const operatorRoutes = ["/numara-kaydi"];

const jwtSecret = new TextEncoder().encode(process.env.JWT_SECRET);

function redirectToLogin(request: NextRequest) {
  const homeUrl = new URL("/anasayfa", request.url);
  return NextResponse.redirect(homeUrl);
}

function redirectToProfile(request: NextRequest) {
  const homeUrl = new URL("/profil", request.url);
  return NextResponse.redirect(homeUrl);
}

export async function middleware(request: NextRequest) {
  const pathname = request.nextUrl.pathname;

  const isPublicRoute = publicRoutes.some((route) => pathname.startsWith(route));

  if (isPublicRoute) return NextResponse.next();

  const token = request.cookies.get("token")?.value;

  if (!token) {
    return redirectToLogin(request);
  }

  try {
    const { payload } = await jwtVerify(token, jwtSecret);
    const decodedToken = payload as DecodedToken;

    const isOperatorRoute = operatorRoutes.some((route) => pathname.startsWith(route));
    const isAdminRoute = adminRoutes.some((route) => pathname.startsWith(route));

    if (isOperatorRoute && !["Operator", "Admin"].includes(decodedToken.rol)) {
      return redirectToProfile(request);
    }

    if (isAdminRoute && decodedToken.rol !== "Admin") {
      return redirectToProfile(request);
    }
  } catch {
    return redirectToLogin(request);
  }

  return NextResponse.next();
}

export const config = {
  matcher: "/((?!api|_next/static|_next/image|favicon.ico|.*\\..*).*)",
};
