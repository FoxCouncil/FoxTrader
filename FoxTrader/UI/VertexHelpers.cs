using System.Collections.Generic;
using System.Drawing;

namespace FoxTrader.UI
{
    public static class VertexHelpers
    {
        public static IReadOnlyList<Vertex> TwoPointsToQuadVertexArray(Point c_firstPoint, Point c_secondPoint, Color c_color)
        {
            var a_lineThickness = 10;
            var a_quadVerticesIndex = 0;
            var a_vertices = new Vertex[4];

            a_vertices[a_quadVerticesIndex].X = (short)c_firstPoint.X;
            a_vertices[a_quadVerticesIndex].Y = (short)(c_firstPoint.Y - a_lineThickness);
            a_vertices[a_quadVerticesIndex].U = 0;
            a_vertices[a_quadVerticesIndex].V = 0;
            a_vertices[a_quadVerticesIndex].R = c_color.R;
            a_vertices[a_quadVerticesIndex].G = c_color.G;
            a_vertices[a_quadVerticesIndex].B = c_color.B;
            a_vertices[a_quadVerticesIndex].A = c_color.A;

            a_quadVerticesIndex++;
            a_vertices[a_quadVerticesIndex].X = (short)c_secondPoint.X;
            a_vertices[a_quadVerticesIndex].Y = (short)(c_secondPoint.Y - a_lineThickness);
            a_vertices[a_quadVerticesIndex].U = 1;
            a_vertices[a_quadVerticesIndex].V = 0;
            a_vertices[a_quadVerticesIndex].R = c_color.R;
            a_vertices[a_quadVerticesIndex].G = c_color.G;
            a_vertices[a_quadVerticesIndex].B = c_color.B;
            a_vertices[a_quadVerticesIndex].A = c_color.A;

            a_quadVerticesIndex++;
            a_vertices[a_quadVerticesIndex].X = (short)(c_secondPoint.X + a_lineThickness);
            a_vertices[a_quadVerticesIndex].Y = (short)c_secondPoint.Y;
            a_vertices[a_quadVerticesIndex].U = 1;
            a_vertices[a_quadVerticesIndex].V = 1;
            a_vertices[a_quadVerticesIndex].R = c_color.R;
            a_vertices[a_quadVerticesIndex].G = c_color.G;
            a_vertices[a_quadVerticesIndex].B = c_color.B;
            a_vertices[a_quadVerticesIndex].A = c_color.A;

            a_quadVerticesIndex++;
            a_vertices[a_quadVerticesIndex].X = (short)(c_firstPoint.X + a_lineThickness);
            a_vertices[a_quadVerticesIndex].Y = (short)c_firstPoint.Y;
            a_vertices[a_quadVerticesIndex].U = 0;
            a_vertices[a_quadVerticesIndex].V = 1;
            a_vertices[a_quadVerticesIndex].R = c_color.R;
            a_vertices[a_quadVerticesIndex].G = c_color.G;
            a_vertices[a_quadVerticesIndex].B = c_color.B;
            a_vertices[a_quadVerticesIndex].A = c_color.A;

            return a_vertices;
        }

        public static IReadOnlyList<Vertex> RectToQuadVertexArray(Rectangle c_rect, float c_u1, float c_v1, float c_u2, float c_v2, Color c_color)
        {
            var a_quadVerticesIndex = 0;
            var a_vertices = new Vertex[4];

            a_vertices[a_quadVerticesIndex].X = (short)c_rect.X;
            a_vertices[a_quadVerticesIndex].Y = (short)c_rect.Y;
            a_vertices[a_quadVerticesIndex].U = c_u1;
            a_vertices[a_quadVerticesIndex].V = c_v1;
            a_vertices[a_quadVerticesIndex].R = c_color.R;
            a_vertices[a_quadVerticesIndex].G = c_color.G;
            a_vertices[a_quadVerticesIndex].B = c_color.B;
            a_vertices[a_quadVerticesIndex].A = c_color.A;

            a_quadVerticesIndex++;
            a_vertices[a_quadVerticesIndex].X = (short)(c_rect.X + c_rect.Width);
            a_vertices[a_quadVerticesIndex].Y = (short)c_rect.Y;
            a_vertices[a_quadVerticesIndex].U = c_u2;
            a_vertices[a_quadVerticesIndex].V = c_v1;
            a_vertices[a_quadVerticesIndex].R = c_color.R;
            a_vertices[a_quadVerticesIndex].G = c_color.G;
            a_vertices[a_quadVerticesIndex].B = c_color.B;
            a_vertices[a_quadVerticesIndex].A = c_color.A;

            a_quadVerticesIndex++;
            a_vertices[a_quadVerticesIndex].X = (short)(c_rect.X + c_rect.Width);
            a_vertices[a_quadVerticesIndex].Y = (short)(c_rect.Y + c_rect.Height);
            a_vertices[a_quadVerticesIndex].U = c_u2;
            a_vertices[a_quadVerticesIndex].V = c_v2;
            a_vertices[a_quadVerticesIndex].R = c_color.R;
            a_vertices[a_quadVerticesIndex].G = c_color.G;
            a_vertices[a_quadVerticesIndex].B = c_color.B;
            a_vertices[a_quadVerticesIndex].A = c_color.A;

            a_quadVerticesIndex++;
            a_vertices[a_quadVerticesIndex].X = (short)c_rect.X;
            a_vertices[a_quadVerticesIndex].Y = (short)(c_rect.Y + c_rect.Height);
            a_vertices[a_quadVerticesIndex].U = c_u1;
            a_vertices[a_quadVerticesIndex].V = c_v2;
            a_vertices[a_quadVerticesIndex].R = c_color.R;
            a_vertices[a_quadVerticesIndex].G = c_color.G;
            a_vertices[a_quadVerticesIndex].B = c_color.B;
            a_vertices[a_quadVerticesIndex].A = c_color.A;

            return a_vertices;
        }
    }
}