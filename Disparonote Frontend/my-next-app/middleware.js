import { NextResponse } from "next/server";

export function middleware(request) {
  const protectedRoutes = ["/create-note"];
  const pathname = request.nextUrl.pathname;

  if (protectedRoutes.some((route) => pathname.startsWith(route))) {
    const token = request.cookies.get("token")?.value;

    if (!token) {
      console.warn("❌ Token is missing. Redirecting to the login page.");
      const url = request.nextUrl.clone();
      url.pathname = "/login";
      return NextResponse.redirect(url);
    }

    try {
      const payloadBase64 = token.split(".")[1];
      const payload = JSON.parse(atob(payloadBase64));

      if (Date.now() >= payload.exp * 1000) {
        console.warn("❌ Token has expired! Redirecting to the login page.");
        const url = request.nextUrl.clone();
        url.pathname = "/login";
        return NextResponse.redirect(url);
      }
    } catch (error) {
      console.error("❌ Token decoding error:", error);
      const url = request.nextUrl.clone();
      url.pathname = "/login";
      return NextResponse.redirect(url);
    }
  }

  return NextResponse.next();
}

export const config = {
  matcher: ["/create-note/:path*"],
};
