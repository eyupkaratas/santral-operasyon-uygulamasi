import { Card, CardContent } from "@/components/ui/card";
import { Dialog, DialogContent, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { cn } from "@/lib/utils";
import { Cargo } from "@/types/cargo";
import { VisuallyHidden } from "@radix-ui/react-visually-hidden";
import CargoCard from "./cargo-card";

type CargoListItemProps = {
  cargo: Cargo;
};

export default function CargoListItem({ cargo }: CargoListItemProps) {
  const isDisabled = cargo.durum === "Beklemede";

  const cardContent = (
    <Card className={cn("rounded-md px-4 py-2", isDisabled ? "cursor-not-allowed opacity-70" : "hover:bg-muted/90")}>
      <CardContent className="flex items-center justify-between p-0">
        <div>
          <div>Gönderen: {cargo.gonderen}</div>
          <div>Takip Numarası: {cargo.takipNumarasi}</div>
        </div>
        <div>{cargo.durum}</div>
      </CardContent>
    </Card>
  );

  if (isDisabled) {
    return cardContent;
  }

  return (
    <Dialog>
      <DialogTrigger asChild>{cardContent}</DialogTrigger>

      <DialogContent className="sm:max-w-md">
        <VisuallyHidden>
          <DialogTitle>Kargo</DialogTitle>
        </VisuallyHidden>

        <CargoCard cargo={cargo} />
      </DialogContent>
    </Dialog>
  );
}
