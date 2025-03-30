/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  async rewrites() {
    return [
      {
        source: "/Auth/:path*",
        destination: "http://disparonote-backend:5000/Auth/:path*",
      },
      {
        source: "/notes/:path*",
        destination: "http://disparonote-backend:5000/api/notes/:path*",
      },
    ];
  },
};

export default nextConfig;
