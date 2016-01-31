// structs.h
// 2014-10-12T23:37:51-07:00

#include <stdint.h>

namespace FoxTrader
{
	struct vec2_t
	{
		uint32_t x;
		uint32_t y;

		public:
			vec2_t()
			{
				this->x = 0;
				this->y = 0;
			}

			vec2_t(uint32_t c_x, uint32_t c_y)
			{
				this->x = c_x;
				this->y = c_y;
			}

			static vec2_t Random(uint32_t c_maxRange)
			{
				uint32_t a_randomX = Tools::RandomNumber(0, c_maxRange);
				uint32_t a_randomY = Tools::RandomNumber(0, c_maxRange);

				return vec2_t(a_randomX, a_randomY);
			}
	};

	struct vec3_t
	{
		uint32_t x;
		uint32_t y;
		uint32_t z;

		public:
			vec3_t()
			{
				this->x = 0;
				this->y = 0;
				this->z = 0;
			}

			vec3_t(uint32_t c_x, uint32_t c_y, uint32_t c_z)
			{
				this->x = c_x;
				this->y = c_y;
				this->z = c_z;
			}

			static vec3_t Random(uint32_t c_maxRange)
			{
				uint32_t a_randomX = Tools::RandomNumber(0, c_maxRange);
				uint32_t a_randomY = Tools::RandomNumber(0, c_maxRange);
				uint32_t a_randomZ = Tools::RandomNumber(0, c_maxRange);

				return vec3_t(a_randomX, a_randomY, a_randomZ);
			}
	};

	struct padding_t
	{
	    uint32_t top;
	    uint32_t right;
	    uint32_t bottom;
	    uint32_t left;

	    public:
            padding_t()
            {
                this->top = 0;
                this->right = 0;
                this->bottom = 0;
                this->left = 0;
            }

            padding_t(uint32_t a_padding)
            {
                this->top = a_padding;
                this->right = a_padding;
                this->bottom = a_padding;
                this->left = a_padding;
            }

            padding_t(uint32_t a_topBottom, uint32_t a_rightLeft)
            {
                this->top = a_topBottom;
                this->right = a_rightLeft;
                this->bottom = a_topBottom;
                this->left = a_rightLeft;
            }

            padding_t(uint32_t a_top, uint32_t a_right, uint32_t a_bottom, uint32_t a_left)
            {
                this->top = a_top;
                this->right = a_right;
                this->bottom = a_bottom;
                this->left = a_left;
            }
	};
}
