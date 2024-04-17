namespace FundParser.BL.DTOs;

public class FundDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<HoldingDTO> Holdings { get; set; } = null!;

    public virtual ICollection<HoldingDiffDTO> HoldingDiffs { get; set; } = null!;
}