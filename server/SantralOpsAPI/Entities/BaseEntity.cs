﻿namespace SantralOpsAPI.Entities;

public abstract class BaseEntity
{
  public int Id { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}