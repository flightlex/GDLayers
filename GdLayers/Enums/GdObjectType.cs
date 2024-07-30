using GdLayers.Attributes;

namespace GdLayers.Enums;

public enum ObjectType
{
    // blocks
    [Title("Solid Blocks")]
    [ResourceReference("SolidBlocks")]
    SolidBlock              = 1,

    [Title("Non-Solid Blocks")]
    [ResourceReference("NonSolidBlocks")]
    NonSolidBlock           = 2,

    // spikes
    [Title("Solid Spikes")]
    [ResourceReference("SolidSpikes")]
    SolidSpike              = 4,

    [Title("Non-Solid Spikes")]
    [ResourceReference("NonSolidSpikes")]
    NonSolidSpike           = 8,
    
    // hazards
    [Title("Animated Hazards")]
    [ResourceReference("AnimatedHazards")]
    AnimatedHazard          = 16,

    [Title("Other Hazards")]
    [ResourceReference("OtherHazards")]
    OtherHazard             = 32,

    // deco
    [Title("Outline Decorations")]
    [ResourceReference("OutlineDecorations")]
    OutlineDecoration       = 64,

    [Title("Filler Decorations")]
    [ResourceReference("FillerDecorations")]
    FillerDecoration        = 128,

    [Title("Animated Decorations")]
    [ResourceReference("AnimatedDecorations")]
    AnimatedDecoration      = 256,

    [Title("Pixel Decorations")]
    [ResourceReference("PixelDecorations")]
    PixelDecoration         = 512,

    [Title("Animated Pixel Decorations")]
    [ResourceReference("AnimatedPixelDecorations")]
    AnimatedPixelDecoration = 1024,

    [Title("Glow Decorations")]
    [ResourceReference("GlowDecorations")]
    GlowDecoration          = 2048,

    [Title("Other Decorations")]
    [ResourceReference("OtherDecorations")]
    OtherDecoration         = 4096,

    // gameplay
    [Title("Launching Pads")]
    [ResourceReference("LaunchingPads")]
    LaunchingPad            = 8192,

    [Title("Orbs")]
    [ResourceReference("Orbs")]
    Orb                     = 16_384,

    [Title("Gravity Portals")]
    [ResourceReference("GravityPortals")]
    GravityPortal           = 32_768,

    [Title("Gamemode Portals")]
    [ResourceReference("GamemodePortals")]
    GamemodePortal          = 65_536,   

    [Title("Size Portals")]
    [ResourceReference("SizePortals")]
    SizePortal              = 131_072,

    [Title("Other Portals")]
    [ResourceReference("OtherPortals")]
    OtherPortal             = 262_144,

    // other gameplay
    [Title("Speed Modifiers")]
    [ResourceReference("SpeedModifiers")]
    SpeedModifier           = 524_288,

    [Title("Special Blocks")]
    [ResourceReference("SpecialBlocks")]
    SpecialBlock            = 1_048_576,

    // saw
    [Title("Saws")]
    [ResourceReference("Saws")]
    Saw                     = 2_097_152,

    // other
    [Title("Collectible Objects")]
    [ResourceReference("Collectibles")]
    Collectible             = 4_194_304,

    [Title("Triggers")]
    [ResourceReference("Triggers")]
    Trigger                 = 8_388_608
}
