import CargoList from "./(components)/cargo-list";

export default function Page() {
  return (
    <div className="flex justify-center">
      <div className="w-full max-w-lg space-y-2">
        <div className="text-center">Kargolarım</div>
        <div className="text-center">Filtre</div>
        <CargoList />
      </div>
    </div>
  );
}
