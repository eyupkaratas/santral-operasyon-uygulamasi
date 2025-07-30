import { Personnel } from "@/types/personnel";
import UnitItem from "./unit-item";

type UnitListProps = {
  personnels: Personnel[];
};

export default function UnitList({ personnels }: UnitListProps) {
  return (
    <div className="space-y-2">
      {personnels.map((personnel) => (
        <UnitItem key={personnel.id} personnel={personnel} />
      ))}
    </div>
  );
}
