import { jwtVerify } from "jose";
import { NextRequest, NextResponse } from "next/server";

const publicRoutes = ["/anasayfa"];
const jwtSecret = new TextEncoder().encode(process.env.JWT_SECRET);

function redirect(request: NextRequest, to: string) {
  const url = new URL(to, request.url);
  return NextResponse.redirect(url);
}

export async function middleware(request: NextRequest) {
  const pathname = request.nextUrl.pathname;

  const isPublic = publicRoutes.some(
    (route) => pathname.startsWith(route) //
  );

  if (isPublic) return NextResponse.next();

  const token = request.cookies.get("token")?.value;

  if (!token) {
    return redirect(request, "/anasayfa");
  }

  try {
    await jwtVerify(token, jwtSecret);
  } catch {
    return redirect(request, "/anasayfa");
  }

  return NextResponse.next();
}

export const config = {
  matcher: "/((?!api|_next/static|_next/image|favicon.ico|.*\\..*).*)",
};
