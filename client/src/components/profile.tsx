import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

type CardItemProps = {
  label: string;
  content: string;
};

function CardItem({ label, content }: CardItemProps) {
  return (
    <div className="flex flex-col items-center justify-center border-b">
      <div className="mb-2 text-center">
        <p className="font-bold">{label}</p>
        <p>{content}</p>
      </div>
    </div>
  );
}

export default function Profile() {
  return (
    <Card>
      <CardHeader className="border-b-1">
        <CardTitle className="mb-4 text-center">Profil</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <CardItem label="İsim, Soyisim" content="Eyüp Karataş" />

        <CardItem label="Birim, Ünvan" content="Bilgi İşlem, Yazılım Mühendisi" />

        <CardItem label="Dahili No" content="2615" />

        <CardItem label="E-Posta" content="eyup.karatas@manisa.bsb.tr" />
      </CardContent>
    </Card>
  );
}
