import Profile from "@/components/profile";
import { Card } from "@/components/ui/card";
import { Dialog, DialogContent, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { VisuallyHidden } from "@radix-ui/react-visually-hidden";

export default function UnitItem() {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Card className="hover:bg-muted/90 rounded-md px-4 py-2">Eyüp Karataş, Yazılım Mühendisi</Card>
      </DialogTrigger>

      <DialogContent className="sm:max-w-md">
        <VisuallyHidden>
          <DialogTitle>Profil</DialogTitle>
        </VisuallyHidden>

        <Profile />
      </DialogContent>
    </Dialog>
  );
}
