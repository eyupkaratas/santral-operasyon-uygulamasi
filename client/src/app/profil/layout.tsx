import Header from "@/components/header";

export default function Layout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <div className="min-h-svh">
      <Header />
      <main className="mx-auto max-w-7xl p-4">{children}</main>
    </div>
  );
}
