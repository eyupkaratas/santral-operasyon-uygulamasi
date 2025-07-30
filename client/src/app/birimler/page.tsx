import SearchBar from "@/components/search-bar";
import UnitList from "./(components)/unit-list";

export default function Page() {
  return (
    <div className="flex justify-center">
      <div className="w-full max-w-lg space-y-2">
        <div className="text-center">Bilgi İşlem</div>
        <SearchBar />
        <UnitList />
      </div>
    </div>
  );
}
