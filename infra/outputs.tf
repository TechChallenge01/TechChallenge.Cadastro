output "cadastro_service_url" {
  description = "URL pública do Cadastro Service"
  value       = "http://${aws_lb.cadastro_service.dns_name}"
}

output "ecr_cadastro_repository_url" {
  description = "URL do repositório ECR do Cadastro Service"
  value       = aws_ecr_repository.cadastro_service.repository_url
}
