int botao = 2;

void setup()
{
  pinMode(botao, INPUT);
  Serial.begin(9600);
}

void loop()
{
  bool leituraBotao = digitalRead(botao);
  Serial.println(leituraBotao);
  delay(100);
}