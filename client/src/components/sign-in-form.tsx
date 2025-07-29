"use client";

import { loginAction } from "@/actions/auth";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { cn } from "@/lib/utils";
import { zodResolver } from "@hookform/resolvers/zod";
import { Loader2 } from "lucide-react";
import { useTransition } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";

const formSchema = z.object({
  email: z.email({ error: "Geçersiz e-posta adresi." }),
  password: z.string().nonempty({ message: "Lütfen bir şifre girin." }),
});

export function SignInForm({ className, ...props }: React.ComponentProps<"div">) {
  const [isPending, startTransition] = useTransition();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  function onSubmit(values: z.infer<typeof formSchema>) {
    startTransition(async () => {
      const res = await loginAction({
        email: values.email,
        password: values.password,
      });

      if (res.success) {
        // TODO
      } else {
        alert(res.message);
      }
    });
  }

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className="text-center">Giriş Yap</CardTitle>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
              <div className="flex flex-col gap-6">
                <FormField
                  name="email"
                  control={form.control}
                  render={({ field }) => (
                    <FormItem>
                      <div className="grid gap-3">
                        <FormLabel>E-Posta</FormLabel>
                        <FormControl>
                          <Input placeholder="eyup.karatas@manisa.bsb.tr" {...field} />
                        </FormControl>
                        <FormMessage />
                      </div>
                    </FormItem>
                  )}
                />

                <FormField
                  name="password"
                  control={form.control}
                  render={({ field }) => (
                    <FormItem>
                      <div className="grid gap-3">
                        <FormLabel>Şifre</FormLabel>
                        <FormControl>
                          <Input type="password" {...field} />
                        </FormControl>
                        <FormMessage />
                      </div>
                    </FormItem>
                  )}
                />

                <div className="flex flex-col gap-3">
                  <Button type="submit" className="w-full">
                    {isPending ? <Loader2 /> : "Giriş Yap"}
                  </Button>
                </div>
              </div>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
