resource "aws_ecr_repository" "cadastro_service" {
  name                 = "${var.project_name}-cadastro-service"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }

  tags = { Name = "cadastro-service" }
}

resource "aws_ecr_lifecycle_policy" "cadastro_service" {
  repository = aws_ecr_repository.cadastro_service.name

  policy = jsonencode({
    rules = [{
      rulePriority = 1
      description  = "Manter apenas as últimas 5 imagens"
      selection = {
        tagStatus   = "any"
        countType   = "imageCountMoreThan"
        countNumber = 5
      }
      action = { type = "expire" }
    }]
  })
}
