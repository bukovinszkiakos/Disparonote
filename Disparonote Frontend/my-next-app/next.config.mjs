/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  async rewrites() {
    return [
      {
        source: "/Auth/:path*",
        destination: "http://localhost:5184/Auth/:path*",
      },
      {
        source: "/notes/:path*",
        destination: "http://localhost:5184/api/notes/:path*",
      },
    ];
  },
};

export default nextConfig;
