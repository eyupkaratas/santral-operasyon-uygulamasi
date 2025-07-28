import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";

type CargoItemProps = {
  label: string;
  content: string;
  textarea?: boolean;
  dropdown?: boolean;
};

function CargoItem({ label, content, textarea, dropdown }: CargoItemProps) {
  let contentElement = <Input defaultValue={content} />;

  if (textarea) {
    contentElement = <Textarea className="w-full [resize:none]" rows={5} defaultValue={content} />;
  }

  if (dropdown) {
    contentElement = (
      <Select>
        <SelectTrigger className="w-full">
          <SelectValue placeholder="Durum" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value="low">Düşük</SelectItem>
          <SelectItem value="medium">Orta</SelectItem>
          <SelectItem value="high">Yüksek</SelectItem>
        </SelectContent>
      </Select>
    );
  }

  return (
    <div className="flex flex-col items-center justify-center border-b">
      <div className="mb-2 w-full space-y-2 text-center">
        <p className="font-bold">{label}</p>
        {contentElement}
      </div>
    </div>
  );
}

export default function Cargo() {
  return (
    <Card>
      <CardHeader className="border-b-1">
        <CardTitle className="mb-4 text-center">Kargo</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <CargoItem label="Gönderen" content="Görderen İsmi" />

        <CargoItem label="Alıcı" content="Alıcı İsmi" />

        <CargoItem label="Durum" content="" dropdown />

        <CargoItem label="Açıklama" content="Lorem ipsum." textarea />
      </CardContent>
    </Card>
  );
}
