import { getPersonnelsAction } from "@/actions/personnel";
import { getUnitsAction } from "@/actions/unit";
import UnitFilter from "./(components)/unit-filter";
import UnitList from "./(components)/unit-list";

export default async function Page({
  searchParams,
}: {
  searchParams: Promise<{ [key: string]: string | string[] | undefined }>;
}) {
  const birimId = (await searchParams).birimId?.toString();

  const personnels = await getPersonnelsAction(birimId);
  const units = await getUnitsAction();

  return (
    <div className="flex justify-center">
      <div className="w-full max-w-lg space-y-2">
        <div className="text-center">Bilgi İşlem</div>
        <UnitFilter units={units} />
        <UnitList personnels={personnels} />
      </div>
    </div>
  );
}
