"use client";

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { DecodedToken } from "@/types/decoded-token";
import { useEffect } from "react";
import { toast } from "sonner";

type CardItemProps = {
  label: string;
  content: string;
};

type ProfileProps = {
  decodedToken: DecodedToken;
};

function CardItem({ label, content }: CardItemProps) {
  return (
    <div className="flex flex-col items-center justify-center border-b">
      <div className="mb-2 text-center">
        <p className="font-bold">{label}</p>
        <p className="whitespace-pre-line">{content}</p>
      </div>
    </div>
  );
}

export default function Profile({ decodedToken }: ProfileProps) {
  const { personalAdSoyad, dahiliNo, birim, unvan, eposta, rol } = decodedToken;

  useEffect(() => {
    toast.success(
      <>
        <p className="text-center">
          Hoş geldin <strong>{personalAdSoyad}</strong>.
        </p>
        <p className="text-center">
          Rolün: <strong>{rol}</strong>
        </p>
      </>,
      {
        closeButton: true,
        position: "top-center",
        classNames: {
          content: "w-full",
        },
      }
    );
  }, []);

  return (
    <Card>
      <CardHeader className="border-b-1">
        <CardTitle className="mb-4 text-center">Profil</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <CardItem label="İsim, Soyisim" content={personalAdSoyad} />

        <CardItem label="Birim, Ünvan" content={`${birim}\n${unvan}`} />

        <CardItem label="Dahili No" content={dahiliNo} />

        <CardItem label="E-Posta" content={eposta} />
      </CardContent>
    </Card>
  );
}
