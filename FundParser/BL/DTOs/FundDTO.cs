namespace BL.DTOs;

public class FundDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<HoldingDTO> Holdings { get; set; }

    public virtual ICollection<HoldingDiffDTO> HoldingDiffs { get; set; }
}