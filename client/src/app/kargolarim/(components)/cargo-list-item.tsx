import { Card, CardContent } from "@/components/ui/card";
import { Dialog, DialogContent, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { VisuallyHidden } from "@radix-ui/react-visually-hidden";
import Cargo from "./cargo";

export default function CargoListItem() {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Card className="hover:bg-muted/90 rounded-md px-4 py-2">
          <CardContent className="flex justify-between p-0">
            <div>Kargo</div>
            <div>Durum</div>
          </CardContent>
        </Card>
      </DialogTrigger>

      <DialogContent className="sm:max-w-md">
        <VisuallyHidden>
          <DialogTitle>Kargo</DialogTitle>
        </VisuallyHidden>

        <Cargo />
      </DialogContent>
    </Dialog>
  );
}
