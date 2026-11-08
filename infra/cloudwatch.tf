resource "aws_cloudwatch_log_group" "cadastro_service" {
  name              = "/ecs/cadastro-service"
  retention_in_days = 7

  tags = { Name = "cadastro-service-logs" }
}
