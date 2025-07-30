import { jwtVerify } from "jose";
import { NextRequest, NextResponse } from "next/server";

const publicRoutes = ["/anasayfa"];
const jwtSecret = new TextEncoder().encode(process.env.JWT_SECRET);

function redirectToHome(request: NextRequest) {
  const homeUrl = new URL("/anasayfa", request.url);
  return NextResponse.redirect(homeUrl);
}

export async function middleware(request: NextRequest) {
  const pathname = request.nextUrl.pathname;

  const isPublic = publicRoutes.some(
    (route) => pathname.startsWith(route) //
  );

  if (isPublic) return NextResponse.next();

  const token = request.cookies.get("token")?.value;

  if (!token) {
    return redirectToHome(request);
  }

  try {
    await jwtVerify(token, jwtSecret);
  } catch {
    return redirectToHome(request);
  }

  return NextResponse.next();
}

export const config = {
  matcher: "/((?!api|_next/static|_next/image|favicon.ico|.*\\..*).*)",
};
